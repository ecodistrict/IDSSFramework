import jsonschema
from jsonschema import validate
import json
import urllib.request
schema_url = "https://raw.githubusercontent.com/ecodistrict/IDSSFramework/master/schema/message.json"
schema = json.loads(urllib.request.urlopen(schema_url).read().decode('utf-8'))
json = {
    "method": "getModules",
    "type": "request"
}


try:
    validate(json, schema)
except jsonschema.exceptions.ValidationError as e:
    print(e)
