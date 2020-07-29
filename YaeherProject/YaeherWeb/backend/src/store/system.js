const systemConfig = {
    state: {
        collapse: false,
        pwdCollapse:false,
        asideWidth:'220px',

    },
    mutations: {
        CHANGE_SET: (state, value) => {
            if(value==undefined){
                state.collapse = !state.collapse;
            }else{
                state.collapse = value ? true : false;
            }
            state.asideWidth = state.collapse?'50px' :'220px';
        },
        PWD_SET:(state,value)=>{
            state.pwdCollapse=value;    
        }    
    },
    actions: {
        setCollapse({ commit },value) {
            commit('CHANGE_SET',value);
        },
        setPwd({commit},value){
            commit('PWD_SET',value);
        }
    }
}

export default systemConfig
