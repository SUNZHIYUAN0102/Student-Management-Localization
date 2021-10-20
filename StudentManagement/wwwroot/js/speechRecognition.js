var Description = document.querySelector("#txtMessage");
var SpeechRecognition = SpeechRecognition || webkitSpeechRecognition;
var SpeechGrammarList = SpeechGrammarList || webkitSpeechGrammarList;
var grammar = '#JSGF V1.0;'
var recognition = new SpeechRecognition();
var speechRecognitionList = new SpeechGrammarList();
speechRecognitionList.addFromString(grammar, 1);
recognition.grammars = speechRecognitionList;
recognition.lang = 'en-US';
recognition.interimResults = true;

recognition.onresult = function (event) {
    let command = event.results[0][0].transcript;
    Description.value = command;
}
document.querySelector('#btnGiveCommand').onclick = function () {
    recognition.start();
};
recognition.onspeechend = function () {
    recognition.stop();
};
recognition.onerror = function (event) {
    Description.value = 'Error occurred in recognition: ' + event.error;
}  