using JSchemaTester.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Data;

namespace JSchemaTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonString = @"{
                'ResponseCode' : 200,
                'ResponseMessage' : 'Data was received',
                'Data':[
                    {
                        'Id' : 147,
                        'FirstName' : 'Bob',
                        'MiddleName' : 'Lawrence',
                        'LastName' : 'Welch',
                        'Age' : 66,
                        'Profession' : 'musician',
                        'Score' : 10,
                        'Address': 'Hollywood, Los Angeles, California, USA'
                    },
                    {
                        'Id' : 18,
                        'FirstName' : 'Steven',
                        'MiddleName' : 'Paul',
                        'LastName' : 'Jobs',
                        'Age' : 56,
                        'Profession' : 'businessman',
                        'Score' : 9,
                        'Address': 'San Francisco, California, USA'
                    },
                    {
                        'Id' : 594,
                        'FirstName' : 'Hanna',
                        'MiddleName' : '',
                        'LastName' : 'Holasava',
                        'Age' : 23,
                        'Profession' : 'developer',
                        'Score' : 10,
                        'Address': 'Gomel, Belarus'
                    },
                    {
                        'Id' : 363,
                        'FirstName' : 'Stephen',
                        'MiddleName' : 'William',
                        'LastName' : 'Hawking',
                        'Age' : 76,
                        'Profession' : 'scientist',
                        'Score' : 7,
                        'Address': 'Oxford, United Kingdom'
                    }
                ]
            }";

            JSchema schema = JSchema.Parse(@"{
              '$schema': 'http://json-schema.org/draft-04/schema#',
              'type': 'object',
		      'additionalProperties' : false,
              'properties': {
                'Data': {
                  'type': 'array',
                  'items': 
                    {
                      'type': 'object',
		              'additionalProperties' : false,
                      'properties': {
                        'FirstName': {
                          'type': 'string'
                        },
                        'LastName': {
                          'type': 'string'
                        },
                        'Age': {
                          'type': 'integer'
                        },
                        'Score': {
                          'type': 'integer'
                        }
                      }
                    }
                }
              }
            }");

            var data = JObject.Parse(jsonString);

            var isValid = data.IsValid(schema, out IList<ValidationError> errors);

            if (!isValid)
            {
                foreach (var error in errors)
                {
                    if (error.ErrorType == ErrorType.AdditionalProperties)
                    {
                        data.SelectToken(error.Path)?.RemoveFromLowestPossibleParent();
                    }
                }
            }

            JToken dataNode = data.SearchNodeByName("Data");

            var dataTable = JsonConvert.DeserializeObject<DataTable>(dataNode.First.ToString());

            dataTable.PrintToConsole();

            Console.ReadKey();
        }
    }
}

