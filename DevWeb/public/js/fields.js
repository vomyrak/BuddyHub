var num_method = 1;
var num_function = 1;

const method_html = "<div>Method: <br>" +
  "&#8195;Method Name: <input type=\"text\" class=\"form-control\" name=\"method\" placeholder=\"Name of the method(e.g. turn on, etc.)\" required/>" +
  "&#8195;HTTP Method: <input type=\"text\" class=\"form-control\" name=\"httpmethod\" placeholder=\"HTTP method (GET, POST, PUT, DELETE)\" required/>" +
  "&#8195;Link: <input type=\"text\" class=\"form-control\" name=\"link\" placeholder=\"Link of the call\" required/>" +
  "&#8195;Data: <input type=\"text\" class=\"form-control\" name=\"data\" placeholder=\"JSON data to be sent\" required/>" +
  "&#8195;Headers: <input type=\"text\" class=\"form-control\" name=\"headers\" placeholder=\"Header of the request\" required/>" +
  "&#8195;Text Input Field: <input type=\"text\" class=\"form-control\" name=\"textinput\" placeholder=\"text input field in the JSON data if required\"/>" +
  "&#8195;Continuous? <input type=\"checkbox\" class=\"form-control\" name=\"continuous\" value=\"true\"><br>" +
  "<input type=\"button\" onclick=\"removeMethod(this)\" style=\"float: right; margin-right: 100px;\" value=\"Delete Method\"/> <br><br></div>";

const function_html = "<div>Function: <br>" +
  "&#8195;Function Name: <input type=\"text\" class=\"form-control\" name=\"function\" placeholder=\"Name of the function in the library(e.g. turnOn, etc.)\" required/>" +
  "&#8195;Continuous? <input type=\"checkbox\" name=\"continuous\" value=\"continuous\"><br>" +
  "<input type=\"button\" onclick=\"removeFunction(this)\" style=\"float: right; margin-right: 100px;\" value=\"Delete Function\" /> <br><br></div>";

function addMethod(button) {
  num_method++;
  $(method_html).appendTo($(".methods"));
}

function removeMethod(button) {
  if (num_method > 1) {
    $(button).parent().remove();
    num_method--;
  }
}

function addFunction(button) {
  num_function++;
  $(function_html).appendTo($(".functions"));
  console.log("called");
}

function removeFunction(button) {
  if (num_function > 1) {
    $(button).parent().remove();
    num_function--;
  }
}
