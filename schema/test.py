import jsonschema
from jsonschema import validate
import json
import urllib.request
#schema_url = "https://raw.githubusercontent.com/ecodistrict/IDSSFramework/master/schema/message.json"
#schema = json.loads(urllib.request.urlopen(schema_url).read().decode('utf-8'))
schema = json.loads(open("message.json", 'rb').read().decode("utf-8"))

json = {
  "method": "moduleResult",
  "type": "result",
  "moduleId": "foo-bar_cheese-module-v1-0",
  "variantId": "503f191e8fcc19729de860ea",
  "kpiId": "cheese-taste-kpi",
  "outputs": [
    {
      "type": "kpi",
      "value": 42,
      "info": "Cheese tastiness",
      "unit": "ICQU (International Cheese Quality Units)"
    }
  ]
}

validate(json, schema)