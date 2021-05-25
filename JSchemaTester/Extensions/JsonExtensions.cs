using Newtonsoft.Json.Linq;

namespace JSchemaTester.Extensions
{
    public static partial class JsonExtensions
    {
        public static JToken RemoveFromLowestPossibleParent(this JToken node)
        {
            if (node == null)
            {
                return null;
            }

            // If the parent is a JProperty, remove that instead of the token itself.
            var property = node.Parent as JProperty;
            var contained = property ?? node;
            if (contained.Parent != null)
            {
                contained.Remove();
            }

            // Also detach the node from its immediate containing property -- Remove() does not do this even though it seems like it should
            if (property != null)
            {
                property.Value = null;
            }
            return node;
        }

        public static JToken SearchNodeByName(this JToken parentNode, string nodeName)
        {
            if (parentNode == null || parentNode.Children().Count() == 0)
            {
                return null;
            }

            if (parentNode is JProperty && string.Equals(((JProperty)parentNode).Name, nodeName))
            {
                return parentNode;
            }

            foreach (JToken children in parentNode.Children())
            {
                var node = SearchNodeByName(children, nodeName);
                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }
    }

}
