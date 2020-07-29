<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
             <div>
                <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" @keyup.enter.native="search"></yh-input>
                <label class="yh-label">查询类型:</label>
                <el-select placeholder="查询类型" v-model="params.totalType" @change="totalquery">
                     <el-option label="日" value="day"></el-option>
                     <el-option label="月" value="month"></el-option>
                     <el-option label="年" value="year"></el-option>
                </el-select>
                <el-select v-if="params.totalType!='day'" placeholder="请选择年份" v-model="year" @change="changeYear">
                     <el-option v-for="(item,key) in yearNum" :key="key" :label="item+'年'" :value="item"></el-option>
                </el-select>
                <el-select v-if="params.totalType=='month'" placeholder="请选择月份" v-model="month" @change="changeMonth">
                     <el-option v-for="(item,key) in monthNum" :key="key" :label="item+'月'" :value="item"></el-option>
                </el-select>
                <el-date-picker v-if="params.totalType=='day'"
                  v-model="day"
                  type="date"
                  value-format="yyyy-MM-dd"
                  @change="changeDate"
                  placeholder="选择日期">
                </el-date-picker>
                <el-button @click="query">查询</el-button>                
            </div>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%">
                <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
                <el-table-column prop="doctorName" label="医生姓名" width="160" align="center"></el-table-column>
                <el-table-column prop="orderTotal" label="订单总数" align="center">
                </el-table-column>
                <el-table-column prop="completeTotal" label="完成订单数" align="center">
                </el-table-column>
                <el-table-column prop="refundTotal" label="退单数量" align="center"> 
                </el-table-column>
                <el-table-column prop="revenueTotal" label="总收入" align="center">
                </el-table-column>
                <el-table-column label="统计时间"  align="center">
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
      year:'',
      month:'',
      day:'',
      yearNum:['2016','2017','2018','2019','2020','2021','2022','2023','2024','2025','2026','2027','2028','2029','2030','2031','2032','2033','2034','2035','2036','2037','2038','2039','2040','2041','2042','2043','2043','2045','2046','2047','2048','2049','2050'],
      monthNum:['01','02','03','04','05','06','07','08','09','10','11','12'],
      params: {//只需要业务参数
        keyWord:'',
        totalType:'month',
        startTime: '',
        endTime: '',
        skipCount:1,
        maxResultCount:10
      }
    }
  },
  methods:{
    totalquery(){
      if(this.params.totalType=='year'){
        this.params.startTime=this.year+'-01-01';
        this.params.endTime=this.year+'-12-31';
      }else if(this.params.totalType=='month'){
        this.monthCheck(this.year,this.month);
      }
    },
    changeYear(){
      if(this.params.totalType=='year'){
        this.params.startTime=this.year+'-01-01';
        this.params.endTime=this.year+'-12-31';
      }else if(this.params.totalType=='month'){
        this.monthCheck(this.year,this.month);
      }
    },
    changeMonth(){
      this.monthCheck(this.year,this.month);
    },
    changeDate(){
      this.params.startTime=this.day;
      this.params.endTime=this.day;
    },
    monthCheck(y,m){
      switch(m){
        case '01':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'31';
          break;
        case '02':
          this.params.startTime=y+'-'+m+'-'+'01';
          if(Number(y)%4==0){
            this.params.endTime=y+'-'+m+'-'+'29';
          }else{
            this.params.endTime=y+'-'+m+'-'+'28';
          }
          break;
        case '03':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'31';
          break;
        case '04':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'30';
          break;
        case '05':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'31';
          break;
        case '06':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'30';
          break;
        case '07':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'31';
          break;
        case '08':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'31';
          break;
        case '09':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'30';
          break;
        case '10':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'31';
          break;
        case '11':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'30';
          break;
        case '12':
          this.params.startTime=y+'-'+m+'-'+'01';
          this.params.endTime=y+'-'+m+'-'+'31';
          break;
      }
    }
  },
  created(){
    let date= new Date();
    this.year=date.getFullYear()+'';
    this.month=(date.getMonth()+1)<10?'0'+(date.getMonth()+1):(date.getMonth()+1);
    this.monthCheck(this.year,this.month);
    this.initTableData(this.$api.ordercountList,this.params);
  }
};
</script>
