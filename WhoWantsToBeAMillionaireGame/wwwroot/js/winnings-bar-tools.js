function selectAllWinnings() {
    let selects = [];
    for (let i = 1; i <= 15; i++) {
        selects.push(document.getElementById(`winning-${i}`));
    }

    return selects;
}

function markCurrentWinnings(number) {
    let select = document.getElementById(`winning-${number}`);
    select.style.background = '#86b0cf';
}

function markSuccessfullWinnings(currentNumber) {    
    if (currentNumber > 1) {
        let selects = selectAllWinnings();
        for (let i = 1; i < currentNumber; i++) {
            selects[i-1].style.background = '#5bc97a';
        }
    }
}

(async () => {
    let currentQuestionNumber = await getCurrentQuestionNumberFromServerAsync();
    markCurrentWinnings(currentQuestionNumber);
    markSuccessfullWinnings(currentQuestionNumber);
})();