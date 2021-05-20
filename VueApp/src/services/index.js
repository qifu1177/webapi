import axios from 'axios';

function HttpData() {
    let that = this;
    this.baseUrl = '',
    this.init = (baseUrl) => that.baseUrl = baseUrl;
    this.get = (url) => {
        return {
            then: (callBack, errorBack) => {
                axios.get(url).then(function (response) {
                    if (typeof callBack === 'function')
                        callBack(response.data);
                }, function (response) {
                    if (typeof errorBack === 'function')
                        errorBack(response)
                });
            }
        }
    };
    this.delete = (url) => {
        return {
            then: (callBack, errorBack) => {
                axios.delete(url).then(function (response) {
                    if (typeof callBack === 'function')
                        callBack(response.data);
                }, function (response) {
                    if (typeof errorBack === 'function')
                        errorBack(response)
                });
            }
        }
    };
    this.uploadFile = (sessionId, inputfile) => {
        return {
            then: (callBack, errorBack) => {
                let url = that.baseUrl + '/file';
                let formData = new FormData();
                formData.append("sessionid", sessionId);
                formData.append("file", inputfile);
                axios.post(url, formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                }).then((response) => {
                    if (typeof callBack === 'function')
                        callBack(response.data);
                }, (response) => {
                    if (typeof errorBack === 'function')
                        errorBack(response)
                });
            }
        }
    };
    this.createFormData = (sessionid, inputData) => {
        let formData = new FormData();
        formData.append("SessionId", sessionid);
        for (let k in inputData) {
            formData.append(k, inputData[k]);
        }
        return formData;
    };
    this.post = (className, sessionid, inputData) => {
        return {
            then: (callBack, errorBack) => {
                let url = that.baseUrl + '/' + className;
                let formdata = this.createFormData(sessionid, inputData);
                axios.post(url, formdata, {
                    headers: {
                        'Content-Type': 'form-data'
                    }
                }).then(function (response) {
                    if (typeof callBack === 'function')
                        callBack(response.data);
                }, function (response) {
                    if (typeof errorBack === 'function')
                        errorBack(response)
                });
            }
        }
    };
    this.put = (className, sessionid, inputData) => {
        return {
            then: (callBack, errorBack) => {
                let url = that.baseUrl + '/' + className;
                let formdata = this.createFormData(sessionid, inputData);
                axios.put(url, formdata, {
                    headers: {
                        'Content-Type': 'form-data'
                    }
                }).then(function (response) {
                    if (typeof callBack === 'function')
                        callBack(response.data);
                }, function (response) {
                    if (typeof errorBack === 'function')
                        errorBack(response)
                });
            }
        }
    };
    this.all = (className) => {
        let url = that.baseUrl + '/all/' + className;
        return that.get(url);
    };
    this.listWithParentId = (className, parentId) => {
        let url = this.baseUrl + '/list/id/' + className + '/' + parentId;
        return that.get(url);
    };
    this.id = (className, id) => {
        let url = this.baseUrl + '/id/' + className + '/' + id;
        return that.get(url);
    };
    this.deleteWithId = (className, id) => {
        let url = this.baseUrl + '/' + className + '/' + id;
        return that.delete(url);
    }
}
export default new HttpData();
