var JaySchema = require('jayschema');
var js = new JaySchema(JaySchema.loaders.http);     // we provide the HTTP loader here
                                                    // you could load from a DB instead

//var instance = { "method": "selectModule", "type": "response" };
var instance = "123aoeu";
var schema = { "$ref": "https://github.com/ecodistrict/IDSSFramework/raw/master/messaging/schema.json#/web-friendly-string" };

js.validate(instance, schema, function(errs) {
  if (errs) { console.error(errs); 
    if (errs[0].subSchemaValidationErrors) {
      console.log(errs[0].subSchemaValidationErrors['#/definitions/getModulesRequest']);
    }}
  else { console.log('validation OK!'); }
});