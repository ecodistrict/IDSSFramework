// The request for dashboard to get the model input specification

var request = {
  "type": "request",
  "method": "selectModel",
  "id": "myModel",
  "kpi": "kpi1"
}

// The reponse from a model to let the dashboard give the correct input, see the inputs specification of the dashboard

var response = {
  "method": "selectModel",
  "type": "response",
  "id": "myModel",
  "inputs": [ 
    {
      // the specification of inputs are constantly being expanded, these are the base properties
      "type": "some input type known to the dashboard",
      "label": "My label for the input",
      "id": "myId", // this id (UNIQUE for your model) is important for you to find the values of the input when the input is returned from dashboard 
      "inputs": [{
        // there can be inputs inside of inputs in some cases (again look at the documentation)        
      }]
    }
  ],
}

