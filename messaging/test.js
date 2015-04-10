var JaySchema = require('jayschema');
var js = new JaySchema(JaySchema.loaders.http);     // we provide the HTTP loader here
                                                    // you could load from a DB instead

var baseURI = "https://github.com/ecodistrict/IDSSFramework/raw/master/messaging/schema.json"

function validate (jsonPointer, instance) {
  var schema = { "$ref": baseURI + jsonPointer };

  js.validate(instance, schema, function(errs) {
    if (errs) {
      console.error(errs);
    } else { 
      console.log('validation OK!');
    }
  });
}

validate("#/message", {"method": "getModules", "type": "request"});

validate(
  "#/inputs/number",
  {"type": "number", "min": 0, "value": 42, "label": "A number"});

validate(
  "#/inputSpecification",
  {
    "list-of-people": {
      "type": "list",
      "label": "Add people and their shoe sizes",
      "inputs": {
        "name": {
          "type": "text",
          "label": "Name"
        },
        "shoe-size": {
          "type": "number",
          "label": "Shoe size",
          "value": 42
        }
      }
    }
  });

