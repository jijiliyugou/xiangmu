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
                <el-table-column prop="doctorName" label="医生姓名" align="center"></el-table-column>
                <el-table-column prop="onlineState" label="上下线状态" align="center">
                  <div slot-scope="scope">
                    <label v-if="scope.row.onlineState=='offline'">下线</label><label v-else>上线</label>
                  </div>
                </el-table-column>
                <el-table-column prop="remark" label="备注说明" align="center"> 
                </el-table-column>
                <el-table-column prop="divideInto" label="分成设置" align="center">
                </el-table-column>
                <el-table-column prop="doctorMoneyExchange" label="资费调价幅度" align="center"> 
                  <div slot-scope="scope">
                    <label v-text="scope.row.doctorMoneyExchange+'%'"></label>
                  </div>
                </el-table-column>
                <el-table-column prop="doctorMoneyexTime" label="资费调价时间" align="center"> 
                  <div slot-scope="scope">
                    <label v-text="scope.row.doctorMoneyexTime"></label>
                  </div>
                </el-table-column>
                <el-table-column prop="incomeDay" label="收入时间设置" align="center"> 
                </el-table-column>
                <el-table-column label="新增时间"  align="center">
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
  },
  created(){
    this.getRangeTime();
    this.initTableData(this.$api.doctorlogList,this.params);
  }
};
</script>
