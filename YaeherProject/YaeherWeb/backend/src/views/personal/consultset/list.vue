<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
          <div><el-button v-if="item.show" v-for="(item,key) in paramArr" :key="key" type="primary" 
              @click="routerLink('/consultset/add',item.code,item.value)"
              >{{item.value+'服务创建'}}</el-button></div>
        </div>
        <!-- 表格 -->
        <div class="yh-container" style="margin-top:24px;">
           <el-table :data="tableData" border style="width: 100%">
                <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
                <el-table-column prop="serviceTypeValue" label="咨询类型" align="center">
                </el-table-column>
                <el-table-column prop="serviceExpense" label="价格" align="center">
                </el-table-column>
                <el-table-column prop="serviceFrequency" label="次数" align="center"></el-table-column>
                <el-table-column label="咨询时长/分钟" align="center">
                  <div slot-scope="scope">
                    <label>{{scope.row.serviceDuration==0?'无':scope.row.serviceDuration}}</label>
                  </div>
                </el-table-column>
                <el-table-column prop="serviceState" label="服务状态" align="center"> 
                  <div slot-scope="scope">
                    <label v-if="scope.row.serviceState==true">开启</label><label v-else>关闭</label>
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button type="edit" @click.native="routerLink('/consultset/update',scope.row.serviceType,scope.row.id)"
                      ></yh-button>
                  </div>
                 </el-table-column>
           </el-table>
        </div>
        <!-- 表格底部 -->
  </section>
</template>

<script>
import listMixins from '@/mixins/list'
export default {
  name:'keepin',
  mixins:[listMixins],
  data() {
    return {
      params: {//只需要业务参数
        keyWord:'',
        startTime: '',
        endTime: '',
        skipCount:1,
        maxResultCount:100
      },
      paramArr:[],
      imgShow:true,
      phoneShow:true,
    }
  },
  methods:{
    getType(){
      this.$http.post(this.$api.consultsetParam).then(res=>{
        if(res.result.code==200){
          this.paramArr=res.result.item;
          for(var j=0;j<this.paramArr.length;j++){
            if(this.paramArr[j].code=='ImageText'){
              this.paramArr[j].show=this.imgShow;
            }else if(this.paramArr[j].code=='Phone'){
              this.paramArr[j].show=this.phoneShow;
            }
          }
        }
      }).catch(error=>{this.errorMessage("获取参数失败")});
    }
  },
  created(){
    
  },
  activated () {
    if(this.params.skipCount==1){
      $(".el-pager .number").eq(0).click();
    }
    this.loading = true;
    this.$http
      .post(this.$api.consultsetList,this.params)
      .then(res => {
          this.loading = false;
          if (res.result.code == '200') {
              this.tableData = res.result.item;
              for(var i=0;i<this.tableData.length;i++){
                if(this.tableData[i].serviceType=='ImageText'){
                  this.imgShow=false;
                }else if(this.tableData[i].serviceType=='Phone'){
                  this.phoneShow=false;
                }
              }
              this.getType();
          } else {
              this.getType();
              this.tableData = [];
          }


      })
      .catch(error => {
          this.loading = false;
      });
  },
  beforeRouteLeave (to, from, next) {
    let topath=to.path.split('/').pop();
    if(topath=='add'||topath=='list'){
      this.params.skipCount=1;
    }
    next();
  }
};
</script>
