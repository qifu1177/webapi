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
    this.setErrorFunc = function (func) {
        this.errorFunc = func;
    }
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
                backData.message = backData.exception.message;
                backData.state = window.$UploadFileState.fail;
                that.errorFunc(response);
            });
    };
    this.loadFiles = (fileList, selectedFiles) => {
        let url = this.baseUrl + '/file/files';
        http.get(url).then((datas) => {
            for (let i = 0; i < datas.length; i++) {
                //datas[i]['link'] = that.baseUrl + '/file/' + datas[i].name;                
                datas[i]['selected'] = selectedFiles.includes(datas[i].name);
                datas[i]['showDelete'] = false;
                fileList.push(datas[i]);
            }
        }, this.errorFunc);
    };
    this.deleteFile = (filename, backData) => {
        let url = this.baseUrl + '/file/' + filename;
        http.delete(url).then((message) => {
            backData.message = message;
        }, (response) => {
            backData.exception = response;
            backData.message = backData.exception.message;
            that.errorFunc(response);
        });
    };
}
export default new Logic();