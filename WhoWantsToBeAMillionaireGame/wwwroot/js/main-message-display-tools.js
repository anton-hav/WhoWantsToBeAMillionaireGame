function selectMessageBox() {
    let messageBox = document.getElementById("message");
    return messageBox;
}

function showInformation(text) {
    let select = selectMessageBox();
    select.innerText = text;
}

function showCurrentInformation(currentQuestionNumber) {
    let text = "Your non-burnable winnings are ";

    let nonBurnableWinning = "";
    if (currentQuestionNumber < 6) {
        nonBurnableWinning = "0 zł";
    } else if (currentQuestionNumber < 11) {
        nonBurnableWinning = "1 000 zł";
    } else {
        nonBurnableWinning = "32 000 zł";
    }

    text = text + nonBurnableWinning;

    showInformation(text);
}

function showWinningInformation() {
    let text = "You win. Your winnings are 1 000 000 zł";
        
    showInformation(text);
}

function showLosserInformation(currentQuestionNumber) {
    let text = "Wasted. Your winnings are ";

    let nonBurnableWinning = "";
    if (currentQuestionNumber < 6) {
        nonBurnableWinning = "0 zł";
    } else if (currentQuestionNumber < 11) {
        nonBurnableWinning = "1 000 zł";
    } else {
        nonBurnableWinning = "32 000 zł";
    }

    text = text + nonBurnableWinning;

    showInformation(text);
}

function showKeepMoneyInformation(currentQuestionNumber) {
    let text = "You didn't make it to the end.. Your winnings are ";

    let winning = "";
    if (currentQuestionNumber == 2) {
        winning = "100 zł";
    } else if (currentQuestionNumber == 3) {
        winning = "200 zł";
    } else if (currentQuestionNumber == 4) {
        winning = "300 zł";
    } else if (currentQuestionNumber == 5) {
        winning = "500 zł";
    } else if (currentQuestionNumber == 6) {
        winning = "1 000 zł";
    } else if (currentQuestionNumber == 7) {
        winning = "2 000 zł";
    } else if (currentQuestionNumber == 8) {
        winning = "4 000 zł";
    } else if (currentQuestionNumber == 9) {
        winning = "8 000 zł";
    } else if (currentQuestionNumber == 10) {
        winning = "16 000 zł";
    } else if (currentQuestionNumber == 11) {
        winning = "32 000 zł";
    } else if (currentQuestionNumber == 12) {
        winning = "64 000 zł";
    } else if (currentQuestionNumber == 13) {
        winning = "125 000 zł";
    } else if (currentQuestionNumber == 14) {
        winning = "250 000 zł";
    } else {
        winning = "500 000 zł";
    }

    text = text + winning;

    showInformation(text);
}