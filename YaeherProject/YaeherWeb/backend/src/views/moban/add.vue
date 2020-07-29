<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
         <el-form-item label="专题名称" class="flex-50" prop="name">
               <el-input v-model="dataInfo.name" class="yh-width"></el-input>
         </el-form-item>
         <el-form-item label="包含栏目" class="flex-50" prop="">
              <el-button type="primary" @click="selectChannel">包含栏目</el-button>
         </el-form-item>
         <el-form-item label="描述" class="flex-100" prop="">
             <el-input v-model="dataInfo.description" class="yh-width" type="textarea"></el-input>
         </el-form-item>
           <el-form-item label="推荐" class="flex-50" prop="">   
             <el-radio-group v-model="dataInfo.recommend" >
                <el-radio :label="true">是</el-radio>
                <el-radio :label="false">否</el-radio>
             </el-radio-group>
         </el-form-item>
          <el-form-item label="排列顺序" class="flex-50" prop="priority">
             <el-input v-model="dataInfo.priority" class="yh-width"></el-input>
         </el-form-item>
         <el-form-item class="flex-50"></el-form-item>
           <el-form-item label="标题图" class="flex-50" prop="">
            <yh-upload :src="dataInfo.titleImg" @change="getTitleImg"></yh-upload>
         </el-form-item>
          <el-form-item label="内容图" class="flex-50" prop="">
              <yh-upload :src="dataInfo.contentImg" @change="getContentImg"></yh-upload>
         </el-form-item>
         <yh-editor :defaultMsg="info['txt']" :index="0" @ready="getUeditor"></yh-editor>
    </el-form>
    <div class="form-footer">
        <el-button type="primary" @click="update()"
               >确定</el-button>
    </div>
   <!-- 移动栏目弹窗 --> 
           <el-dialog class="dialog" :title="labelDialogTitle" :visible.sync="channelVisble" width="25%"> 
              <div class="tree-layout">
                <div class="tree">
                  <el-tree :data="treeData"
                           :indent="16" 
                           :props="channelProps"
                           ref="channelTree"
                           node-key="id"
                           :default-expand-all="true"
                           :default-checked-keys="defaultKeys"
                           show-checkbox 

                         @check-change="checkChange" 
                     >
                  </el-tree>
                </div>
              </div>
            <span slot="footer" class="dialog-footer">
              <el-button @click="cancel">返回</el-button>
              <el-button type="primary" @click="channelVisble = false">确认</el-button> 
            </span>
       </el-dialog>            
</section>
</template>
<script type="text/javascript" src="./static/ueditor/ueditor.config.js" id="configScriptTag"></script>
<script type="text/javascript" src="./static/ueditor/ueditor.all.js" id="editorScriptTag"></script>
<script>
import axios from "axios";
import va from "@/rules";
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns], 
  data() {
    let required = va.required();
    let number = va.number();
    return {
      rules: {//校验规则
        name: [required],
        priority: [required, number]
      },
      info:{txt:'zhejsls'},
      tplList:[],
      treeData: [{
          name:'根栏目',
          id: "",
          child: []
        }],
      defaultKeys: [],
      channelVisble: false, //选择栏目弹窗,
      labelDialogTitle: "",
      currentCheckChannelId: 0, //当前选中栏目
      channelProps: {
        label: "name",
        children: "child",
        isLeaf: "hasChild",
        id: "id"
      }
    };
  },
  methods: {
    selectChannel() {
      this.labelDialogTitle = "选择栏目";
      this.channelVisble = true;
     
    },
    cancel() {
      this.channelVisble = false;
      this.$refs['channelTree'].setCheckedKeys([]);
    },
    checkChange(node, checkStatus, childStatus) {
       let checkIds=this.$refs['channelTree'].getCheckedKeys();
        this.dataInfo.channelIds=checkIds;
    },
    getDataInfo(id) {
      //重写获取表单数据
      let api = this.$api; //API地址
      axios.all([
        axios.post(api.topicGet, { id: id }),
        axios.post(api.topicTplList)
      ]).then(axios.spread((res,tpl)=>{
            this.loading = false;
            this.dataInfo = res.body; //将用户数据复制给dataInfo
            this.defaultKeys=res.body.channelIds;
            this.tplList=tpl.body;
            this.$refs["form"].resetFields();
      })) .catch(error => {
          this.loading = false;
        });
    },
    getChannels() {
      axios
        .post(this.$api.fullTextSearchChannelList, { hasContentOnly: true })
        .then(res => {
          this.treeData[0].child = res.body;
        });
    },
   
    getTitleImg(src) {
      this.dataInfo.titleImg = src;
    },
    getContentImg(src) {
      this.dataInfo.contentImg = src;
    },
    add(state) {
      
     this.dataInfo.channelIds=this.dataInfo.channelIds.join(',');//选中的栏目
      this.saveDataInfo(state, this.$api.topicSave, this.dataInfo, "list");
    },
    update() {
  
      //this.dataInfo.channelIds=this.dataInfo.channelIds.join(',');//选中的栏目
      this.updateDataInfo(this.$api.topicUpdate, this.dataInfo, "list");
    },
    handleClose(done){
            done();
    },
    getUeditor(ue,index){
         this.ue[index]=ue;//获取ue实例 
    }
  },
  created() {
    //初始获取数据
    this.getDataInfo(this.id);
     this.getChannels(); 
    /*axios
        .all([
          axios.post(this.$api.referReson), //退单理由
          axios.post(this.$api.referPartdoctor), //推荐医生 
        ])
        .then(
          axios.spread((reson,doctor) => {
            this.resonType=reson.result.item;//退单理由
            const arr=doctor.result.item;
            const newArr=[];
            for(let i=0;i<=arr.length-1;i++){
               const obj={};
               obj.label=arr[i].clinicInfomation.clinicName;
               obj.value=arr[i].clinicInfomation.id;
               if(arr[i].clinicDoctorReltion.length>0){
                 obj.children=[];
                 for(let j=0;j<=arr[i].clinicDoctorReltion.length-1;j++){
                   const childA={};
                   childA.label=arr[i].clinicDoctorReltion[j].doctorName;
                   childA.value=arr[i].clinicDoctorReltion[j].doctorID;
                   obj.children.push(childA);
                 }
               }
               newArr.push(obj);
            }
            this.options=newArr; 
            this.loading=false;
          })
        )
        .catch(err => {
          this.errorMessage('获取参数失败');
          this.loading = false;
        });*/
  }
};
</script>

<style >
.dialog .tree-layout {
  min-height: 400px;
  width: 90%;
}
.dialog .tree-layout .tree {
  width: 100%;
  height: 100%;
  overflow: auto;
  position: absolute;
  border-right: 0px solid #d4dde2;
}
.el-dialog__body {
  min-height: 450px;
  overflow: auto;
}
.red-color {
  color: red;
}
</style>
