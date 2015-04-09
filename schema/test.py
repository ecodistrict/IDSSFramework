import jsonschema
from jsonschema import validate
import json
import urllib.request
schema_url = "https://raw.githubusercontent.com/ecodistrict/IDSSFramework/master/schema/inputSpecification.json"
schema = json.loads(urllib.request.urlopen(schema_url).read().decode('utf-8'))
#schema = json.loads(open("message.json", 'rb').read().decode("utf-8"))

json = {
  "method": "selectModule",
  "type": "response",
  "moduleId": "foo-bar_cheese-module-v1-0",
  "kpiId": "cheese-taste-kpi",
  "variantId": "503f191e8fcc19729de860ea",
  "inputSpecification": {
    "age": {
      "label": "Age",
      "min": 0,
      "type": "number",
      "unit": "years"
    },
    "cheese-type": {
      "label": "Cheese type",
      "type": "select",
      "value": "brie-cheese",
      "options": [
        {
          "value": "alp-cheese",
          "label": "Alpk\u00e4se"
        },
        {
          "value": "edam-cheese",
          "label": "Edammer"
        },
        {
          "value": "brie-cheese",
          "label": "Brie"
        }
      ]
    }
  }
}
json = {
    "aoeu": "3",
    "age": {
      "label": "Age",
      "min": 0,
      "type": "number",
      "unit": "years"
    },
    "cheese-type": {
      "label": "Cheese type",
      "type": "select",
      "value": "brie-cheese",
      "options": [
        {
          "value": "alp-cheese",
          "label": "Alpk\u00e4se"
        },
        {
          "value": "edam-cheese",
          "label": "Edammer"
        },
        {
          "value": "brie-cheese",
          "label": "Brie"
        }
      ]
    }
  }
validate(json, schema)