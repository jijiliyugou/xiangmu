import axios from "axios";
export default {
    data() {
        return {
            loading: true,//遮罩
            id: this.$route.query.id,//id
            type: this.$route.query.type,//操作类型
            dataInfo: {},//基础表单信息
            form: this.$refs['form']//
        }
    },
    methods: {
        /**获取编辑数据
         * @param {string} url 添加方法的api地址 
         * @param {object} params 添加的参数
         */
        getDataInfo(url, params) {
            this.loading = true;
            axios.post(url, params)
                .then(res => {  

                    if (res.result.code == "200") {
                        this.dataInfo=res.result.item;
                    }
                    this.loading = false;
                }).catch(error => { this.loading = false; })
        },
        /**
         * @param {booealn} state 是否继续添加
         * @param {string} url 添加方法的api地址 
         * @param {object} params 添加的参数
         * @param {string} backUrl ，返回列表的路由地址
         */
        saveDataInfo(url, params, backUrl) {
            let form = this.$refs['form'];
            form.validate((valid) => {//验证方法
                if (valid) {
                    this.loading = true;
                    axios.post(url, params)
                        .then(res => {       
                            if (res.result.code == "200") {
                                this.successMessage('添加成功');
                                setTimeout(() => {
                                    this.loading = false;
                                    this.routerLink(backUrl);
                                }, 1000);
                            }
                        }).catch(error => { this.loading = false; })
                } else {
                    return false
                }
            })
        },
        updateDataInfo(url, params, backUrl) {
            let form = this.$refs['form'];
            form.validate((valid) => {//验证方法
                if (valid) {
                    this.loading = true;
                    delete params.secret;
                    axios.post(url, params)
                        .then(res => {
                            if (res.result.code == "200") {
                                this.successMessage('修改成功');
                                if(backUrl==''){
                                    return false;
                                }else{
                                    setTimeout(() => {
                                        this.loading = false;
                                        this.routerLink(backUrl);
                                    }, 1000);
                                }
                                
                            }
                        }).catch(error => { this.loading = false; })
                } else {
                    return false
                }
            })
        },
        handleDataInfo(url, params, backUrl) {
            let form = this.$refs['form'];
            form.validate((valid) => {//验证方法
                if (valid) {
                    this.loading = true;
                    axios.post(url, params)
                        .then(res => {
                            if (res.result.code == "200") {
                                this.successMessage('操作成功');
                                if(backUrl==''){
                                    return false;
                                }else{
                                    setTimeout(() => {
                                        this.loading = false;
                                        this.routerLink(backUrl);
                                    }, 1000);
                                }
                                
                            }
                        }).catch(error => { this.loading = false; })
                } else {
                    return false
                }
            })
        },
        /**
         * 
         * @param {string} type 判断添加还是修改 save，update 
         */
        isType(type) {
            return this.type == type ? true : false;
        },
        reset() {
            this.getDataInfo(this.id);
        }
    }

}