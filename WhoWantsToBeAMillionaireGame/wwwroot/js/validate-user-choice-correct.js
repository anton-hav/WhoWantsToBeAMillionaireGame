async function validateUserChoice(userChoice) {
    let correctAnswerId = await getCorrectAnswerIdForCurrentQuestion();
    let isCorrect = await getAnswerIsCorrectFromServerByUserChoiceAsync(userChoice);

    if (isCorrect != undefined) {
        await paintButtons(userChoice, isCorrect, correctAnswerId);
        hideGiveMeMyMoneyButtonButton();
        if (userChoice == correctAnswerId) {
            gameQuestion = await getCurrentQuestionNumberFromServerAsync();
            if (gameQuestion == 15) {
                hideNextStepButtonButton();
                showWinningInformation();
                showWinButtonButton();
            } else {
                showNextStepButtonButton();
            }
        } else {
            showGameOverButton();
            let currentQuestionNumber = await getCurrentQuestionNumberFromServerAsync();
            showLosserInformation(currentQuestionNumber);
        }
    } else {
        showGiveMeMyMoneyButtonButton();
    }
    
}

async function paintButtons(userChoice, isCorrect, correctAnswerId) {
    let selects = selectAllButtons();
    for (let i = 0; i < selects.length; i++) {
        if (selects[i].value == userChoice) {
            if (isCorrect == true) {
                selects[i].style.background = '#5bc97a';
            } else {
                selects[i].style.background = '#d16666';
            }
        } else {
            if (isCorrect !== true && selects[i].value == correctAnswerId) {
                selects[i].style.background = '#5bc97a';
            }
        }
    }
}

function isUserChoiceCorrect(userChoice) {
    switchDisableStateForAnswerButtons(true);    
    (async () => await validateUserChoice(userChoice))();
}

(async () => {
        let isTookMoney = await checkIsUserTookMoneyOnServerAsync();
    if (isTookMoney) {
        giveMeMyMoney()
    } else {
        let userChoice = window.getUserChoiceFromModel();
        if (userChoice !== "00000000-0000-0000-0000-000000000000") {
            isUserChoiceCorrect(userChoice);
        } else {
            gameQuestion = await getCurrentQuestionNumberFromServerAsync();
            if (gameQuestion > 1) {
                showGiveMeMyMoneyButtonButton();
            }
        }
    }
        
})();