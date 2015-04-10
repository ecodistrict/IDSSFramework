var JaySchema = require('jayschema');
var js = new JaySchema(JaySchema.loaders.http);     // we provide the HTTP loader here
                                                    // you could load from a DB instead

var baseURI = "https://github.com/ecodistrict/IDSSFramework/raw/master/messaging/schema.json"
var instance = { "type": "select", "oink": "boink", "label": "åpa åpa", "options": [{"label": "aoeu", "value": ""}, {"label": "aoeu", "value": "31"}] };
var schema = { "$ref": baseURI + "#/input" };

js.validate(instance, schema, function(errs) {
  if (errs) {
    console.error(errs);
  } else { 
    console.log('validation OK!');
  }
});