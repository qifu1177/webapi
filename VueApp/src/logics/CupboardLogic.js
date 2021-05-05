import logic from '@/logics/Logic'
function CupboardLogic() {    
    this.loadCupboards = (list) => {
        this.data.all('Cupboard').then(function (datas) {
            list.splice(0, list.length);
            for (let i = 0; i < datas.length; i++) {
                let item = datas[i];
                item.TS = new Date(item.TS);
                list.push(item);
            }
        }, this.errorFunc);
    };
    return logic;
}
CupboardLogic.prototype = logic;
export default new CupboardLogic();