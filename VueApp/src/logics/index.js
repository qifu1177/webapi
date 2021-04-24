import data from '@/services'
function RoomLogic() {
    
    return {
        init: (baseUrl) => {            
            data.init(baseUrl);
        },
        loadRooms: (list) => {
            data.all('Room').then(function (datas) {
                list.splice(0, list.length);
                for (let i = 0; i < datas.length; i++) {
                    let item = datas[i];
                    item.TS = new Date(item.TS);
                    list.push(item);
                }
            }, function (response) {
                    window.console.log(response);
            });
        }
    }
}
export default RoomLogic();