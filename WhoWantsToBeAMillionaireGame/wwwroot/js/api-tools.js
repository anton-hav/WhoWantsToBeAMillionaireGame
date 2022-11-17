async function getAnswerIsCorrectFromServerByUserChoiceAsync(userChoice) {
    let url = new URL(`${window.location.origin}/Game/IsUserChoiceCorrect`);

    let response = await fetch(url,
        {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userChoice)
        });
    return await response.json();
}

async function getCorrectAnswerIdForCurrentQuestion() {
    let url = new URL(`${window.location.origin}/Game/GetCorrectAnswerId`);

    let response = await fetch(url,
        {
            method: 'GET'
        });
    return await response.json();
}

async function markQuestionAsSuccessfulOnServerAsync() {
    let url = new URL(`${window.location.origin}/Game/MarkGameCurrentQuestionAsSuccessful`);

    let response = await fetch(url,
        {
            method: 'GET'
        });
    return await response.json();
}

async function getNextQuestionFromServerAsync() {
    let url = new URL(`${window.location.origin}/Game/GetNextGameQuestion`);

    let response = await fetch(url,
        {
            method: 'GET'
        });
    return await response.json();
}

async function getCurrentQuestionNumberFromServerAsync() {
    let url = new URL(`${window.location.origin}/Game/GetCurrentQuestionNumber`);

    let response = await fetch(url,
        {
            method: 'GET'
        });
    return await response.json();
}

async function checkIsUserTookMoneyOnServerAsync() {
    let url = new URL(`${window.location.origin}/Game/IsUserTookMoney`);

    let response = await fetch(url,
        {
            method: 'GET'
        });
    return await response.json();
}

async function TookMoneyOnServerAsync() {
    let url = new URL(`${window.location.origin}/Game/TookMoney`);

    let response = await fetch(url,
        {
            method: 'GET'
        });
    return await response.json();
}
