import http from '@/services'
window.$UploadFileState = { free: 0, run: 1, successful: 2, fail: 3 };
function Logic() {
    this.baseUrl = "";
    let that = this;
    this.init = (baseUrl) => {
        this.baseUrl = baseUrl;
        http.init(baseUrl + '/data');
    };
    this.data = http;
    this.errorFunc = function (response) {
        window.console.log(response);
    };
    this.loadSessionId = function (session) {
        this.data.get(this.data.baseUrl + '/sessionid').then((sessionId) => session.id = sessionId, this.errorFunc);
    };
    this.uploadFile = (sessionId, inputfile, backData) => {
        this.data.uploadFile(sessionId, inputfile).then((message) => {
            backData.message = message;
            if (message == "OK")
                backData.state = window.$UploadFileState.successful;
            else
                backData.state = window.$UploadFileState.fail;
        }
            , (response) => {
                backData.exception = response;
                window.console.log(response);
                backData.state = window.$UploadFileState.fail;
            });
    };
    this.loadFiles = (fileList) => {
        let url = this.baseUrl + '/file/files';
        http.get(url).then((datas) => {
            for (let i = 0; i < datas.length; i++) {
                datas[i]['link'] = that.baseUrl + '/file/' + datas[i].name;
                fileList.push(datas[i]);
            }
        }, this.errorFunc);
    };
    this.deleteFile = (filename, backData) => {
        let url = this.baseUrl + '/file/' + filename;
        http.delete(url).then((message) => {
            backData.message = message;
        }, this.errorFunc);
    };
}
export default new Logic();