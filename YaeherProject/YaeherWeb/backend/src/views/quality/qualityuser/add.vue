<template>
  <section v-loading="loading" class="yh-body">
        <!-- 返回组件 -->
        <yh-back></yh-back> 
        <div class="yh-list-header flex-between">
             <div>
                <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" @keyup.enter.native="search"></yh-input>
                <el-button @click="query">查询</el-button>                
            </div>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%">
                <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
                <el-table-column label="头像" width="80" align="center">
                  <div slot-scope="scope">
                     <img :src="scope.row.userImage" class="userimages">
                  </div>
                </el-table-column>
                <el-table-column prop="doctorName" label="医生名称" width="160" align="center"></el-table-column>
                <el-table-column prop="title" label="职称" align="center">
                </el-table-column>
                <el-table-column prop="department" label="科室" align="center">
                </el-table-column>
                <el-table-column prop="hospitalName" label="就职医院" align="center">
                </el-table-column>
                <el-table-column prop="onlineState" label="上下线状态" width="80" align="center">
                   <div slot-scope="scope">
                     <label v-if="scope.row.onlineState=='online'">上线</label><label v-else="scope.row.onlineState=='offline'">下线</label>
                  </div>
                </el-table-column>
                <el-table-column prop="wechatNum" label="微信号" align="center">
                </el-table-column>
                <el-table-column prop="orderTotal" label="订单总数" align="center">
                </el-table-column>
                <el-table-column prop="refundTotal" label="退单总数" align="center">
                </el-table-column>
                <el-table-column prop="revenueTotal" label="订单总额" align="center">
                </el-table-column>
                <el-table-column prop="doctorLevel" label="综合评分" align="center">
                </el-table-column>
                <!-- <el-table-column label="状态" width="120" align="center">
                  <div slot-scope="scope">
                    <label v-if="scope.row.qualityState">启用</label><label v-else>停用</label>
                  </div>
                </el-table-column> -->
                <el-table-column label="新增时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
                  </div>
                </el-table-column>
                <el-table-column label="设置为质控委员" align="center">
                  <div slot-scope="scope">
                    <el-button v-if="scope.row.qualityControlId==0" type="success"  @click="qualityset(scope.row.id,true)"
                       >设置质控委员</el-button>
                    <el-button type="success" @click="qualityset(scope.row.qualityControlId,false)" v-else
                       >取消质控委员</el-button>
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
    qualityset(id,type){
      if(type){
        this.$http.post(this.$api.chargepersonAdd,{doctorID:id}).then(res=>{
          if(res.result.code==200){
            this.successMessage("设置质控委员成功");
            this.initTableData(this.$api.chargepersonData,this.params);
          }
        }).catch(error=>{
          this.errorMessage("操作失败");
        });
      }else{
        this.$http.post(this.$api.chargepersonDelete,{id:id}).then(res=>{
          if(res.result.code==200){
            this.successMessage("取消质控委员成功");
            this.initTableData(this.$api.chargepersonData,this.params);
          }
        }).catch(error=>{
          this.errorMessage("操作失败");
        });
      }
    }
  },
  created(){
    this.initTableData(this.$api.chargepersonData,this.params);
  }
};
</script>
