

function SelectTime(currentTime, previousTime) {
    var foundCurrent = false;
    var foundPrevious = false;
    var buttonTags = document.getElementsByTagName("button");

    if (previousTime == "")
        foundPrevious = true;

    for (var i = 0; i < buttonTags.length; i++) {
        if (buttonTags[i].textContent == currentTime) {
            buttonTags[i].style.backgroundColor = '#75E900' // change color to green
            foundCurrent = true;
        }
        else if (buttonTags[i].textContent == previousTime) {
            buttonTags[i].style.backgroundColor = '#FFFFFF' // change color to white
            foundPrevious = true;
        }
        if (foundCurrent && foundPrevious) {
            return true;
        }
    }

    return false;
}


