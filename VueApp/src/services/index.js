import axios from 'axios'

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
    this.all = (className) => {
        let url = that.baseUrl + '/all/' + className;
        return that.get(url);
    };
    this.listWithParentId = (className, parentId) => {
        let url = this.baseUrl + '/list/id/' + className + '/' + parentId;
        return that.get(url);
    };
    this.id = (className, id) => {
        let url = this.baseUrl + '/id/' + className+'/'+id;
        return that.get(url);
    }
}
export default new HttpData();
