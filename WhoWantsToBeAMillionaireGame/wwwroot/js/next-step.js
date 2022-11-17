async function showNewQuestion() {
    hideNextStepButtonButton();
    showGiveMeMyMoneyButtonButton();
    setPropertyForAnswerButtonsByDefault();
    let isMarkSucceed = await markQuestionAsSuccessfulOnServerAsync();
    let gameQuestion;
    if (isMarkSucceed) {
        gameQuestion = await getNextQuestionFromServerAsync();
        if (gameQuestion !== undefined) {
            displayNewQuestion(gameQuestion.question.text)
            setValueOfNewQuestion(gameQuestion.question)
            let currentQuestionNumber = await getCurrentQuestionNumberFromServerAsync();
            showCurrentInformation(currentQuestionNumber);
            markCurrentWinnings(currentQuestionNumber);
            markSuccessfullWinnings(currentQuestionNumber);
        }
        else {
            showWinningInformation();
            showWinButtonButton();
        }

    }
}

function displayNewQuestion(questionText) {
    let display = document.getElementById("question");
    display.innerHTML = questionText;
}

function setPropertyForAnswerButtonsByDefault() {
    let selects = selectAllButtons();
    for (let i = 0; i < selects.length; i++) {
        selects[i].style.background = "";
    }

    switchDisableStateForAnswerButtons(false);
}