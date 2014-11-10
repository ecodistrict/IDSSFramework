// Message specification for communicating with dashboard through framework.
// Date: 10/11 - 2014

// (aimed for demo in Valencia, specification will change!)

// Dashboard subscribes to “dashboard” and publishes on “models”. Module would do the other way round

// Get Models

// Dashboard sends on start up

{
    "method": "getModels",
    "type": "request",
    "parameters": {
    	"kpiList": "kpi1" // not used, right now all modules listening on this request will return itself
	}
}

// Module listen and return
{
	"method": "getModels",
  	"type": "response",
  	"name": "Module name",
  	"id": "moduleId", // must be unique in framework
  	"description": "Description about the module",
  	"kpiList": ["kpi1"] // a list of kpi aliases that the model delivers, the kpi alias is generated from the kpi name in the kpi repository and must be unique 
}

// Select Model

// When user select a model to use for calculating a kpi
{ 
    "method": "selectModel",
    "type": "request",
    "moduleId": "moduleId", // this is why your module knows if the message is relevant
    "kpi": "kpiAlias" // just in case your module delivers more than one kpi. One of these selection messages per kpi delivered
}

// Module returns the input specification - this determines what the module needs from user to calculate
{
    "method": "selectModel",
    "type": "response",
    "moduleId": "moduleId",
    "inputs": [] // This array contains the input specification. The components that can be used will be added after demand from modules we connect
} 

// Start Model

// When user request a calculation
{
	"method": "startModel",
	"type": "request",
	"moduleId": "moduleId",
	"variantId": "variantId",
	"kpiAlias": "kpiAlias",
	"inputs": [] // an object that contains the inputs defined in selectModel.inputs with value properties filled by dashboard usage
}

// When module has started to process the input
{
	"method": "startModel",
	"type": "response",
	"moduleId": "module1",
 	"status": "processing", // could be "processing", "starting", "error", dont know exactly? But module could send more status messages during calculation
  	"kpiAlias": "kpiAlias",
  	"variantId": "variantId" // this is provided from request above, startModel
}

// Model Result
{
	"method": "modelResult",
	"type": "result",
	"moduleId": "moduleId",
	"variantId": "variantId", // this is provided by startModel
	"kpiAlias": "kpiAlias",
	"outputs": [] // This array contains the output specification. The components that can be used will be added after demand from modules we connect
}


