function getModelos(id) { 
$.ajax({
url: "@Url.Action("SubDepto", "Recepcion")",
data: {id:id},
dataType: "json",
type: "POST",
error: function(){
alert("Ha ocurrido un error.");
},
success: function(data){
var items = "<option value=> Seleccione </option>";
$.each(data, function(i, item){
items += "<option value=\"" +
item.Value + "\">" +
item.Text + "</option>";
});
$("#Subdepartamento").html(items);
$("#Subdepartamento").html(items);
}
});