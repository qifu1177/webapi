import http from '@/services'
function Logic() {
    this.init = (baseUrl) => {
        http.init(baseUrl);
    };
    this.data = http;
    this.errorFunc = function (response) {
        window.console.log(response);
    };
    this.loadSessionId = function (session) {
        this.data.get(this.data.baseUrl + '/sessionid').then((sessionId) => session.id = sessionId, this.errorFunc);
    };
    this.uploadFile = (sessionId, inputfile, backData) => {
        this.data.uploadFile(sessionId, inputfile).then((message) => backData.message = message, (response) => backData.exception = response);
    }
}
export default new Logic();