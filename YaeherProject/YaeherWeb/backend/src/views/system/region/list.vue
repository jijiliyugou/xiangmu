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
    <div class="yh-container">
        <el-table :data="tableData" border style="width: 100%">
          <el-table-column label="ID" align="center" prop="ID">
          </el-table-column>
          <el-table-column label="名称" align="center" prop="name">
          </el-table-column>
          <el-table-column label="备注" align="center" prop="remark">
          </el-table-column>
          <el-table-column label="邮编" align="center" prop="postcode">
          </el-table-column>
          <el-table-column label="时间" align="center">
            <div slot-scope="scope">
              {{formatTime(scope.row.createdOn)}} 
            </div>
          </el-table-column>
        </el-table>
    </div>
    <!-- 表格底部 -->
    <div class="yh-list-footer">
            <div class="yh-left">                 
            </div>
            <!-- 分页组件 -->
          <yh-pagination :total="total" @change="getPages"></yh-pagination>
    </div> 
  </section>
</template>
<script>
import listMixins from '@/mixins/list'
import axios from "axios";
export default {
  name: 'region',
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
    this.initTableData(this.$api.regionList,this.params);
  }
}
</script>