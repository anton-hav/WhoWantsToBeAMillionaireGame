function switchDisableStateForAnswerButtons(switcher) {
    let selects = selectAllButtons();

    if (switcher != undefined && switcher == false) {
        for (let i = 0; i < selects.length; i++) {
            selects[i].removeAttribute("disabled");
        }
    } else {
        for (let i = 0; i < selects.length; i++) {
            selects[i].setAttribute("disabled", "");
        }
    }
}

function selectAllButtons() {
    let selects = [];
    selects.push(document.getElementById("answer-A"));
    selects.push(document.getElementById("answer-B"));
    selects.push(document.getElementById("answer-C"));
    selects.push(document.getElementById("answer-D"));

    return selects;
}

function setValueOfNewQuestion(question) {
    let selects = selectAllButtons();
    for (let i = 0; i < selects.length; i++) {
        selects[i].value = question.answers[i].id;
        selects[i].children[0].children[1].innerText = question.answers[i].text; 
    }
}

