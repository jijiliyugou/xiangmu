export default {
    data() {
        return {
            loading: true, //遮罩层
            disabled: true, //禁用状态
            pagetype:'',//有无分页
            id: "", //多选id
            status: "", //审核状态
            tableData: [], //表格数据
            total: 0, //列表总数
            listUrl: '',//列表地址,
            params: {},
            checkedList: [],//选中的对象
            state: false,
        }
    },
    methods: {
        initTableData(url, params,page) {
            this.params = params;
            this.listUrl = url;
            if(page=='nopage'){
                this.getTableDataNoPage(params)
            }else{
                this.getTableData(params)
            }
        },
        getTableData(params) {//获取表格数据分页   
            this.loading = true;
            this.state = false;
            this.$http
                .post(this.listUrl, params)
                .then(res => {
                    this.loading = false;
                    if (res.result.code == '200') {
                        if (res.result.item.totalCount != undefined) {
                            this.total = res.result.item.totalCount;
                        }
                        this.tableData = res.result.item.items;
                        this.state = true;

                    } else {
                        this.tableData = [];
                        this.total=0;
                        this.state = true;
                    }


                })
                .catch(error => {
                    this.loading = false;
                    this.state = false;
                });
        },
        getTableDataNoPage(params) {//获取表格数据没有分页   
            this.loading = true;
            this.state = false;
            this.$http
                .post(this.listUrl, params)
                .then(res => {
                    this.loading = false;
                    if (res.result.code == '200') {
                        this.tableData = res.result.item;
                        this.state = true;

                    } else {
                        this.tableData = [];
                        this.state = true;
                    }


                })
                .catch(error => {
                    this.loading = false;
                    this.state = false;
                });
        },
        getPages(skipCount, maxResultCount) {
            //获取翻页数据
            this.params.skipCount = skipCount;
            this.params.maxResultCount = maxResultCount;
            this.getTableData(this.params);
        },
        search(e){
          var keyCode = window.event? e.keyCode:e.which;
          //console.log('回车',keyCode,e);
          if(keyCode == 13){
            /*if(this.params.keyWord!=undefined){
               if(this.params.keyWord.trim()==""){
                 this.current = 1;
               }
            }*/
            this.query(this.pagetype);
          }
        },
        rangeTime(val){//格式化时间
          if(val!=null){
               this.params.startTime=val[0];
               this.params.endTime=val[1]; 
                if(val[0]===val[1]){
                    this.time=val[0];   
                }else{
                    this.time=val[0]+'-'+val[1];   
                }    
          }else{
            this.params.startTime='';
            this.params.endTime=''; 
            //this.time=this.date.year+'-'+this.date.month+'-'+this.date.day  
          }       
        },
        getRangeTime(){//获取七天时间段
          let start=new Date();
          let end=new Date();
          start.setTime(end.getTime()-60*60*1000*24*31);
          this.params.startTime=start.getFullYear() + '-' + ((start.getMonth() + 1)<10?'0'+(start.getMonth() + 1) : (start.getMonth() + 1))+ '-' + (start.getDate()<10?'0'+start.getDate():start.getDate());
          this.params.endTime=end.getFullYear() + '-' + ((end.getMonth() + 1)<10?'0'+(end.getMonth() + 1) : (end.getMonth() + 1))+ '-' + (end.getDate()<10?'0'+end.getDate():end.getDate());
          this.dateArr=[new Date(this.params.startTime),new Date(this.params.endTime)]
        },

        query(type) { //条件查询
            if(type=='nopage'){
                this.getTableDataNoPage(this.params);
            }else{
                this.params.skipCount=1;
                $(".el-pager .number").eq(0).click();
                /*if(this.params.keyWord!=undefined){
                   if(this.params.keyWord.trim()==""){
                     this.current = 1;
                   }
                }*/
                this.getTableData(this.params);
            }
        },
        checkIds(val) {

            let ids = [];
            for (let i in val) {
                ids.push(val[i].id);
            }
            this.id = ids.join(",");
            this.disabled = val.length > 0 ? false : true;
            this.checkedList = val;
        },
        //获取选中id和状态status
        checkIdsAndStatus(val) {

            let ids = [];
            let status = [];
            for (let i in val) {
                ids.push(val[i].id);
                status.push(val[i].status);
            }
            this.id = ids.join(",");
            this.status = status.join(",");
            this.disabled = val.length > 0 ? false : true;
            this.checkedList = val;
        },
        deleteBatch(url, ids,type) { //删除
            this.$confirm('是否确定删除？', '警告', { type: "error" })
                .then(mes => {
                    this.$http.post(url, { id: ids }).then(res => {
                        if (res.result.code == "200") {
                            this.successMessage('删除成功');
                            if(type=='nopage'){
                                this.getTableDataNoPage(this.params);
                            }else{
                                this.getTableData(this.params);
                            }
                        }
                    });
                })
                .catch(error => { });
        },
        backBatch(url, ids) { //退单
            this.$confirm('是否确定退单？', '警告', { type: "error" })
                .then(mes => {
                    this.$http.post(url, { id: ids }).then(res => {
                        if (res.result.code == "200") {
                            this.successMessage('退单成功');
                            this.getTableData(this.params);
                        }
                    });
                })
                .catch(error => { });
        },
        manageact(url, ids ,status) { //是否激活
            this.$http.post(url, { ids: ids ,status:status}).then(res => {
                if (res.result.code == "200") {
                    this.successMessage('操作成功');
                    //this.getTableData(this.params);
                }
            })
            .catch(error=>{this.errorMessage('操作失败');});
        },
        removeBatch(url, roleId, ids) { //移除
            this.$confirm('确定移除吗？', '提示', { type: "error" })
                .then(mes => {
                    this.$http.post(url, { roleId: roleId, userIds: ids }).then(res => {
                        if (res.result.code == "200") {
                            this.successMessage('移除成功');
                            this.getTableData(this.params);
                        }
                    });
                })
                .catch(error => { });
        },
        priorityBatch(url, ids, priorities, regDefId) { //保存排列循序
            this.$confirm('是否保存排列顺序', '提示', { type: "warning" })
                .then(mes => {
                    this.$http.post(url, { id: ids, priorities: priorities, regDefId: regDefId }).then(res => {
                        if (res.result.code == "200") {
                            this.successMessage('操作成功');
                            this.getTableData(this.params);
                        }
                    });
                })
                .catch(error => { });
        },
        prioritysBatchs(url, ids, priorities, disableds, regDefId) { //保存排列循序四个参数
            this.$confirm('是否保存排序？', '提示', { type: "warning" })
                .then(mes => {
                    this.$http.post(url, { id: ids, priorities: priorities, disableds: disableds, defId: regDefId }).then(res => {
                        if (res.result.code == "200") {
                            this.successMessage('操作成功');
                            this.getTableData(this.params);
                        }
                    });
                })
                .catch(error => { });
        },
        prioritysBatch(url, ids, priorities) { //保存排列
            this.$confirm('是否保存排序？', '提示', { type: "warning" })
                .then(mes => {
                    this.$http.post(url, { id: ids, priorities: priorities }).then(res => {
                        if (res.result.code == "200") {
                            this.successMessage('保存成功');
                            this.getTableData(this.params);
                        }
                    });
                })
                .catch(error => { });
        },
        revertBatch(url, ids) { //批量还原
            this.$confirm('是否确定还原？', '提示', { type: "warning" })
                .then(mes => {
                    this.$http.post(url, { id: ids }).then(res => {
                        if (res.result.code == "200") {
                            this.successMessage('还原成功');
                            this.getTableData(this.params);
                        }
                    });
                })
                .catch(error => { });
        },
        reviewBatch(url, ids) {//批量审核
            this.$confirm('是否批量审核', '提示', { type: "warning" })
                .then(mes => {
                    this.$http.post(url, { id: ids }).then(res => {
                        if (res.result.code == "200") {
                            this.successMessage('审核成功!');
                            setTimeout(() => {
                                this.getTableData(this.params);
                            }, 800);
                        }
                    });
                })
                .catch(error => { });
        }
    }
}