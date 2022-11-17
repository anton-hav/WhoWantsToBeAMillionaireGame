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

function showGameOverButton() {

    let button = document.getElementById('game-over-button');
    button.removeAttribute('hidden');
}

function showNextStepButtonButton() {

    let button = document.getElementById('next-step-button');
    button.removeAttribute('hidden');
}

function hideNextStepButtonButton() {

    let button = document.getElementById('next-step-button');
    button.setAttribute('hidden', '');
}

function showWinButtonButton() {

    let button = document.getElementById('win-button');
    button.removeAttribute('hidden');
}

function showGiveMeMyMoneyButtonButton() {

    let button = document.getElementById('give-me-my-money');
    button.removeAttribute('hidden');
}

function hideGiveMeMyMoneyButtonButton() {

    let button = document.getElementById('give-me-my-money');
    button.setAttribute('hidden', '');
}

function giveMeMyMoney() {
    (async () => {
        let isSuccees = await TookMoneyOnServerAsync();
        let currentQuestionNumber = await getCurrentQuestionNumberFromServerAsync();
        showKeepMoneyInformation(currentQuestionNumber);
        showGameOverButton();
        switchDisableStateForAnswerButtons("disabled");
        hideGiveMeMyMoneyButtonButton();
    })();
}
