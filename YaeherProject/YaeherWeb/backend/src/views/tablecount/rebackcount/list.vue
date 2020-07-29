<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
             <div>
                <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" @keyup.enter.native="search"></yh-input>
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
                <el-table-column prop="refundNumber" label="退单编号" align="center"> 
                </el-table-column>
                <el-table-column prop="doctorName" label="医生姓名" align="center"></el-table-column>
                <el-table-column prop="patientName" label="患者姓名" align="center">
                </el-table-column>
                <el-table-column prop="refundReason" label="退单原因" align="center">
                  <div slot-scope="scope">
                    {{scope.row.refundReason==null?'无':JSON.parse(scope.row.refundReason).LabelName}} 
                  </div> 
                </el-table-column>
                <el-table-column prop="refundRemarks" label="退单备注" align="center"> 
                </el-table-column>
                <el-table-column prop="orderMoney" label="订单金额" align="center"> 
                </el-table-column>
                <el-table-column prop="orderCurrency" label="支付币别" align="center"> 
                </el-table-column>
                <el-table-column label="审核状态" align="center"> 
                  <div slot-scope="scope">
                    {{checkType(scope.row.checkState)}}
                  </div> 
                </el-table-column>
                <el-table-column label="退单时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
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
    checkType(val){
      if(val=='checking'){
        return '审核中';
      }else if(val=='success'){
        return '已通过';
      }else if(val=='fail'){
        return '不通过';
      }
    }
  },
  created(){
    this.getRangeTime();
    this.initTableData(this.$api.rebackcountList,this.params);
  }
};
</script>
