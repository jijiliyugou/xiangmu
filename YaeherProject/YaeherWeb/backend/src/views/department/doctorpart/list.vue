<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
             <div>
                <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" @keyup.enter.native="search"></yh-input>
                <!-- <label class="yh-label">审核状态：</label>
                <el-select placeholder="审核状态" v-model="params.systemType">
                     <el-option label="全部状态" :value="''"></el-option>
                     <el-option v-for="(item,key) in doctorstatusChecked" :key="key" :value="item.code" :label="item.name"></el-option>
                </el-select> -->
                <el-button @click="query">查询</el-button>                
            </div>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%">
                <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
                <el-table-column prop="doctorName" label="医生名称" align="center"></el-table-column>
                <el-table-column prop="onlineState" label="上下线状态" width="80" align="center">
                   <div slot-scope="scope">
                     <label v-if="scope.row.onlineState=='Online'">开启</label><label v-else="scope.row.onlineState=='Offline'">关闭</label>
                  </div>
                </el-table-column>
                <el-table-column prop="divideInto" label="分成设置" width="80" align="center">
                </el-table-column>
                <el-table-column prop="incomeDay" label="回款天数" width="80" align="center">
                </el-table-column>
                <el-table-column prop="doctorMoneyExchange" label="价格浮动值" width="80" align="center">
                </el-table-column>
                <el-table-column prop="doctorMoneyexTime" label="限制天数" width="80" align="center">
                </el-table-column>
                <el-table-column prop="remark" label="备注说明" align="center">
                </el-table-column>
                <el-table-column prop="checkState" label="审核状态" width="80" align="center">
                </el-table-column>
                <el-table-column prop="checkRemark" label="审核备注" align="center">
                </el-table-column>
                <el-table-column label="审核时间" align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.checkTime)}} 
                  </div>
                </el-table-column>
                <el-table-column label="新增时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button type="edit" @click.native="routerLink('/doctorstatus/update','update',scope.row.id)"
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
import axios from "axios";
export default {
  name:'keepin',
  mixins:[listMixins],
  data() {
    return {
      dateArr:'',
      doctorstatusChecked:[],
      params: {//只需要业务参数
        keyWord:'',
        skipCount:1,
        maxResultCount:10
      }/*,
      paramStatus:{
        type:'ConfigPar',
        systemCode:'DoctorCheckRes'
      }*/
    }
  },
  methods:{
  },
  created(){
    
  },
  activated () {
    if(this.params.skipCount==1){
      $(".el-pager .number").eq(0).click();
    }
    this.initTableData(this.$api.doctorstatusList,this.params);
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
