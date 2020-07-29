<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
            <el-button v-if="!isType('quality')" type="primary" 
             icon="el-icon-plus"
              @click="routerLink('/qualityuser/add','add',0)"
              >添加</el-button>
            <!-- 右操作 -->
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
                <el-table-column label="头像" width="80" align="center">
                  <div slot-scope="scope">
                     <img :src="scope.row.userImage==null?'./../static/images/userlogo.png':scope.row.userImage.replace('cos.ap-guangzhou','picgz')+imgRez" class="userimages">
                  </div>
                </el-table-column>
                <el-table-column prop="doctorName" label="医生名称" width="160" align="center"></el-table-column>
                <el-table-column prop="title" label="职称" align="center">
                </el-table-column>
                <el-table-column prop="department" label="科室" align="center">
                </el-table-column>
                <el-table-column prop="hospitalName" label="就职医院" align="center">
                </el-table-column>
                <el-table-column prop="workYear" label="工作年限" align="center">
                </el-table-column>
                <el-table-column prop="orderTotal" label="订单数量" align="center">
                </el-table-column>
                <el-table-column prop="refundTotal" label="退单数量" align="center">
                </el-table-column>
                <el-table-column prop="phoneNumber" label="手机号" align="center">
                </el-table-column>
                <el-table-column prop="wechatNum" label="微信号" align="center">
                </el-table-column>
                <el-table-column prop="accomplish" label="擅长" align="center">
                </el-table-column>
                <el-table-column label="新增时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                     <yh-button title="转给质控委员" v-if="isType('quality')" type="send" @click.native="send(id,scope.row.id,scope.row.doctorName)"
                     ></yh-button>
                     <yh-button v-if="!isType('quality')" type="delete" @click.native="deleteBatch($api.chargepersonDelete,scope.row.qualityControlId)"
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
import formMixns from "@/mixins/form";
export default {
  mixins:[listMixins,formMixns],
  data() {
    return {
      imgRez:localStorage.getItem('imRez')==undefined?'':localStorage.getItem('imRez'),
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
    send(id,doctorId,name){
      this.$confirm('是否转给质控委员:'+name+'？', '警告', { type: "error" })
        .then(mes => {
            this.$http.post(this.$api.chargedoctor,{consultID:id,doctorID:doctorId}).then(res=>{
              if(res.result.code==200){
                this.successMessage("操作成功");
              }
            }).catch(error=>{this.errorMessage("操作失败");});
        })
        .catch(error => { });
    }
  },
  created(){
    this.getRangeTime();
    this.initTableData(this.$api.chargepersonList,this.params);
  }
};
</script>
