import { http, data } from '@/services'
window.$UploadFileState = { free: 0, run: 1, successful: 2, fail: 3 };
function Logic() {    
    let that = this;
    this.http = http;
    this.data = data;
    this.init = (baseUrl) => {        
        http.init(baseUrl);
        data.init(baseUrl);
        return this;
    };
    
    this.errorFunc = function (response) {
        window.console.log(response);
    };
    this.setErrorFunc = function (func) {
        this.errorFunc = func;
    }    
    this.uploadFile = (sessionId, inputfile, backData) => {
        let url = http.baseUrl + '/file';
        http.uploadFile(url,sessionId, inputfile).then((message) => {
            backData.message = message;
            if (message == "OK")
                backData.state = window.$UploadFileState.successful;
            else
                backData.state = window.$UploadFileState.fail;
        }
            , (response) => {
                backData.exception = response;
                backData.message = backData.exception.response.data || backData.exception.message;
                backData.state = window.$UploadFileState.fail;
                that.errorFunc(backData);
            });
    };
    //this.loadFileSetting = (setting) => {
    //    let url = http.baseUrl + '/file/setting';
    //    http.get(url).then((obj) => {            
    //        setting["fileTypes"] = obj.filetypes;
    //        setting["maxSize"] = obj.maxsize;
    //    }, this.errorFunc);
    //}
    this.loadFiles = (sessionid,fileList, selectedFiles) => {
        let url = http.baseUrl + '/file/files/' + sessionid;
        http.get(url).then((datas) => {
            for (let i = 0; i < datas.length; i++) {                         
                datas[i]['selected'] = selectedFiles.includes(datas[i].name);
                datas[i]['showDelete'] = false;
                fileList.push(datas[i]);
            }
        }, this.errorFunc);
    };
    this.deleteFile = (filename, backData) => {
        let url = http.baseUrl + '/file/' + filename;
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