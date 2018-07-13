SET json-file-path=.\ServiceClient\TwitterUgly.json
curl https://badapi.iqvia.io/swagger/v1/swagger.json > %json-file-path%
nswag swagger2csclient /input:%json-file-path% /classname:TwitterUglyServiceClient /namespace:Coding.ServiceClient /output:.\ServiceClient\TwitterUglyServiceClient.cs