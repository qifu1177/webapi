import logic from '@/logics/Logic'
function RoomLogic() {    
    this.loadRooms = (sessionId, list) => {
        let that = this;
        this.data.all('Room', sessionId).then(function (data) {
            if (data.message == "OK") {
                list.splice(0, list.length);
                for (let i = 0; i < data.datas.length; i++) {
                    let item = data.datas[i];
                    item.ts = new Date(item.ts);
                    item.actions = '';
                    item['showDelete'] = false;
                    list.push(item);
                }
            } else {
                that.errorFunc(data.message);
            }
            that.changeUpdateTs(data.lastUpdateMs);
        }, that.errorFunc);
    };    
    this.deleteRoom = (roomId, output) => {
        let that = this;
        this.data.deleteWithId("Room", roomId).then(function (data) {            
            if (data.message == "OK") {
                output.message = data.message;
            } else {
                that.errorFunc(data.message);
            }
            that.changeUpdateTs(data.lastUpdateMs);
        }, that.errorFunc);
    };
    this.saveRoom = (sessionid, inputRoom, output) => {
        let that = this;
        if (parseInt(inputRoom.RoomId) === 0) {
            this.data.post('Room', sessionid, inputRoom).then(function (data) {
                if (data.message == "OK") {
                    output.message = data.message;
                } else {
                    that.errorFunc(data.message);
                }
                that.changeUpdateTs(data.lastUpdateMs);
            }, that.errorFunc);
        } else {
            this.data.put('Room', sessionid, inputRoom).then(function (data) {
                if (data.message == "OK") {
                    output.message = data.message;
                } else {
                    that.errorFunc(data.message);
                }
                that.changeUpdateTs(data.lastUpdateMs);
            }, that.errorFunc);
        }
    }
}
RoomLogic.prototype = logic;
export default new RoomLogic();