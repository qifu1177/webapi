export default {
    namespaced: true,
    state: {
        SessionId: '',
        LastUpdateTs: new Date(),
        UserName: '',
        Modules:[],
        duration:60
    },
    getters: {
        SessionId:(state)=> {
            return state.SessionId;
        },
        UserName: (state) => {
            return state.UserName;
        },
        LastUpdateMs(state) {
            return state.LastUpdateTs.getTime();
        },        
        StateTimeCheck: (state) => {
            let delta = (new Date()).getTime() - state.LastUpdateTs.getTime();
            return delta <= state.duration * 1000;           
        },
        IsLogined: (state) => {
            return state.SessionId != "";
        }
    },
    mutations: {
        SessionId:(state, sessionId)=> {
            state.SessionId = sessionId;
        },
        LastUpdateTs:(state,ts)=> {
            state.LastUpdateTs = ts;
        },  
        Login: (state, session) => {
            state.SessionId = session.data.sessionId;
            state.LastUpdateTs = new Date(session.data.loginTs);
            state.UserName = session.data.userName;
            state.Modules = session.data.modules;
            window.sessionStorage.setItem(session.LoginInfo, JSON.stringify(session.data));
            session.afterLogin();
        },
        Logout: (state,session) => {
            state.SessionId = '';
            state.LastUpdateTs = new Date();
            state.UserName = '';
            state.Modules = [];
            window.sessionStorage.removeItem(session.LoginInfo);
            session.Logout();
        }
    },
    actions: {
        LoginCheck: ({ commit, getters, state }, session) => {
            state.duration = session.duration;            
            let b = state.SessionId && getters.StateTimeCheck;
            if (b) {
                return;
            } else {
                commit("SessionId", '');
                commit("LastUpdateTs", new Date());
                session.toLogin();
            }
        },
        Logout: async ({ commit, getters, dispatch },session) => {
            window.setTimeout(() => {
                let delta = (new Date()).getTime() - getters.LastUpdateMs;
                if (delta > (session.duration * 1000)) {
                    commit("Logout", session);                    
                } else {
                    dispatch("Logout", session);
                }
            }, 60000)
        }
    }
}