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
                <el-table-column prop="refundNumber" label="退单编号" width="160" align="center"></el-table-column>
                <!-- <el-table-column prop="orderNumber" label="订单编号" align="center"> 
                </el-table-column> -->
                <el-table-column prop="consultNumber" label="咨询单号" align="center"> 
                </el-table-column>
                <el-table-column prop="consultantName" label="咨询姓名" align="center"> 
                </el-table-column>
                <el-table-column prop="patientName" label="患者姓名" align="center"> 
                </el-table-column>
                <el-table-column prop="doctorName" label="医生姓名" align="center"> 
                </el-table-column>
                <el-table-column prop="iiInessDescription" label="问题描述" align="center" :show-overflow-tooltip="true">
                </el-table-column>
                <el-table-column prop="refundTime" label="退单时间" align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.refundTime)}} 
                  </div>
                </el-table-column>
                <el-table-column prop="refundType" label="退单类型" align="center"> 
                  <div slot-scope="scope">
                    {{typeCheck(scope.row.refundType)}} 
                  </div>
                </el-table-column>
                <!-- <el-table-column prop="refundState" label="退单状态" align="center"> 
                </el-table-column> -->
                <el-table-column prop="refundRemarks" label="退单理由" align="center" :show-overflow-tooltip="true"> 
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button type="view" @click.native="routerLink('/retreat/view','view',scope.row.refundNumber)"
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
    typeCheck(val){
      if(val=="patientreturn"){
        return '患者退单';
      }else if(val=="doctorreturn"){
        return '医生退单';
      }else if(val=="qualityreturn"){
        return '质控退单';
      }else if(val=="systemreturn"){
        return '系统退单';
      }
    }
  },
  created(){
    
  },
  activated () {
    this.getRangeTime();
    if(this.params.skipCount==1){
      $(".el-pager .number").eq(0).click();
    }
    this.initTableData(this.$api.retreatList,this.params);
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
