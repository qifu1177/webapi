import axios from 'axios';

function Http() {   
    this.baseUrl = '';
    this.init = (baseUrl) => this.baseUrl = baseUrl;
    this.get = (url,header) => {
        return {           
            then: (callBack, errorBack) => {
                axios.get(url,header).then(function (response) {
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
    this.uploadFile = (url,sessionId, inputfile) => {
        return {
            then: (callBack, errorBack) => {                
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
    
    this.post = (url,formData) => {
        return {
            then: (callBack, errorBack) => {               
                axios.post(url, formData, {
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
    this.put = (url,formData) => {
        return {
            then: (callBack, errorBack) => {                
                axios.put(url, formData, {
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
}
export default new Http();