<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
             <div>
                <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'"></yh-input>
                <el-date-picker
                    v-model="dateArr"
                    value-format="yyyy-MM-dd"
                    :editable='false'
                    @change="rangeTime"
                    type="daterange"
                    range-separator="至"
                    start-placeholder="开始日期"
                    end-placeholder="结束日期">
                </el-date-picker>
                <el-button @click="query">查询</el-button>                
            </div>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%">
                <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
                <el-table-column prop="appid" label="appid" align="center"> 
                </el-table-column>
                <el-table-column prop="backUrl" label="backUrl" align="center"> 
                </el-table-column>
                <el-table-column prop="consultNumber" label="咨询单号" width="160" align="center"></el-table-column>
                <el-table-column prop="consultantName" label="咨询姓名" align="center"> 
                </el-table-column>
                <el-table-column prop="doctorName" label="医生姓名" align="center"> 
                </el-table-column>
                <el-table-column prop="miniprogram" label="miniprogram" align="center"> 
                </el-table-column>
                <el-table-column prop="msgID" label="msgID" align="center"> 
                </el-table-column>
                <el-table-column prop="msgType" label="msgType" align="center"> 
                </el-table-column>
                <el-table-column prop="pagepath" label="pagepath" align="center"> 
                </el-table-column>
                <el-table-column prop="templateCode" label="templateCode" align="center"> 
                </el-table-column>
                <el-table-column prop="templateId" label="templateId" align="center">
                </el-table-column>
                <el-table-column prop="toUser" label="toUser" align="center"> 
                </el-table-column>
                <el-table-column label="新增时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button type="view" @click.native="routerLink('/wechatnewrecord/view','view',scope.row.id)"
                      ></yh-button>
                  </div>
                 </el-table-column>
           </el-table>
        </div>
        <!-- 表格底部 -->
        <div class="yh-list-footer">
                <div class="yh-left">                 
                  <!-- <el-button :disabled="disabled" @click="deleteBatch($api.topicDelete,ids)"                 
                  >批量删除</el-button> -->
                </div>
                <!-- 分页组件 -->
              <yh-pagination :total="total" @change="getPages"></yh-pagination>
        </div> 
  </section>
</template>

<script>
import listMixins from '@/mixins/list'
export default {
  name:'keepin',
  mixins:[listMixins],
  data() {
    return {
      dateArr:'',
      params: {//只需要业务参数
        keyWord:'',
        startTime: '',
        endTime: '',
        skipCount:1,
        maxResultCount:10
      }
    }
  },
  methods:{
  },
  created(){
    
  },
  activated () {
    this.getRangeTime();
    if(this.params.skipCount==1){
      $(".el-pager .number").eq(0).click();
    }
    this.initTableData(this.$api.WXNewsSendList,this.params);
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
