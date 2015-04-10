var JaySchema = require('jayschema');
var js = new JaySchema(JaySchema.loaders.http);     // we provide the HTTP loader here
                                                    // you could load from a DB instead

var baseURI = "https://github.com/ecodistrict/IDSSFramework/raw/master/messaging/schema.json"

function validate (instance, jsonPointer) {
  var schema = { "$ref": baseURI + jsonPointer };

  js.validate(instance, schema, function(errs) {
    if (errs) {
      console.error(errs);
    } else { 
      console.log('validation OK!');
    }
  });
}


var instance = {
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
};

validate(instance, "#/inputSpecification")