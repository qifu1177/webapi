import logic from '@/logics/Logic'
function RoomLogic() {    
    this.loadRooms = (list) => {
        this.data.all('Room').then(function (datas) {
            list.splice(0, list.length);
            for (let i = 0; i < datas.length; i++) {
                let item = datas[i];
                item.TS = new Date(item.TS);
                item.actions = '';
                item['showDelete'] = false;
                list.push(item);
            }
        }, this.errorFunc);
    };    
    this.deleteRoom = (roomId, output) => {
        this.data.deleteWithId("Room", roomId).then(function (message) {
            output.message = message;
        }, this.errorFunc);
    };
    this.saveRoom = (sessionid, inputRoom,output) => {
        if (parseInt(inputRoom.RoomId) === 0) {
            this.data.post('Room', sessionid, inputRoom).then(function (message) {
                output.message = message;
            }, this.errorFunc);
        } else {
            this.data.put('Room', sessionid, inputRoom).then(function (message) {
                output.message = message;
            }, this.errorFunc);
        }
    }
}
RoomLogic.prototype = logic;
export default new RoomLogic();