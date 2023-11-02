var cardTitle = Array.from(document.querySelectorAll('.card-title'));
var cardText = Array.from(document.querySelectorAll('.card-text'));

cardTitle.forEach(function (element) {
    var description = element.innerHTML;
    element.innerHTML = (description.length > 30) ? description.substring(0, 30) + '...' : description
});
cardText.forEach(function (element) {
    var description = element.innerHTML;
    element.innerHTML = (description.length > 90) ? description.substring(0, 90) + '...' : description
});
