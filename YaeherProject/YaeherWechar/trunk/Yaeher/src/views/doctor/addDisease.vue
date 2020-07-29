<template>
    <div class="add-disease padding-top">
        <mt-header fixed title="添加病种">
            <a @click="$router.push({ path: '/disease'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content add-box">
            <div class="add-case">
                <!-- <p class="disease-title">病种</p> -->
                <mt-field label="症状（疾病）" placeholder="请输入症状或疾病" v-model="lableName"></mt-field>
                
            </div>
            <!-- <ul class="labelListCase">
                <li class="hint">
                    点击病种添加病种
                </li>
                <li @click="createlable(item.lableName)" v-for="(item, index) in labelList" :key="index">
                    {{item.lableName}}
                </li>
            </ul> -->
            <div @click="createlable" class="okCase">
                <mt-button type="primary" class="mint-button--large">保存</mt-button>
            </div>
        </div>
    </div>
</template>

<script>
import { Toast, MessageBox } from 'mint-ui';

export default {
    data () {
        return {
            id: 0,
            doctorID: 0,
            lableName: '',
            labelList: {}
        }
    },
    methods: {
        // createlable(lableName1) { // 添加病种
        //     MessageBox.confirm(`您确定添加 (${lableName1}) 病种？`).then(action => {
        //         const lableName = lableName1
        //         this.instance.createLableD({
        //             lableName
        //         })
        //             .then((response) => {
        //                 Toast('添加成功')
        //                 this.$router.push({ 
        //                     path: '/disease'
        //                 })
        //             })
        //             .catch((error) => {
        //             }) 
        //     },function(){
        //         console.log('取消了');
        //     })
        // },
        createlable () { // 添加病种
            const lableName = this.lableName
            if (!lableName) {
                Toast('病种不能为空')
                return
            }

            if (lableName.length > 15) {
                Toast('病种不能超过15个字')
                return
            }

            this.instance.createLableD({
                lableName
            })
                .then((response) => {
                    if(response.data.result.code===200) {
                        Toast('添加成功')
                        this.$router.push({ 
                            path: '/disease'
                        })
                    }
                })
                .catch((error) => {
                }) 
        },
        getLabelList() { // 获取病种列表
            let doctorID =  this.doctorID
            this.instance.doctorLables({
                doctorID
            })
                .then((response) => {
                    if (response.data.result.item) {
                        this.labelList = response.data.result.item
                    } else {
                        this.labelList = []
                    }
                    
                })
                .catch((error) => {
                }) 
        }
    },
    created () {
        this.doctorID = parseInt(window.sessionStorage.getItem('userId'))
        // this.getLabelList()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.add-disease {
    .add-box {
        .add-case {
            margin: 10px; 
            padding: 10px 15px; 
            background: $color-wfont;
            p {line-height: 30px;}
        }
        .okCase {
            margin: 20px 10px;
            padding: 0;
        }
    }
    .labelListCase {
        li {
            padding: 15px 10px;
        }
        .hint {
            font-size: $font-xl;
            text-align: center;
            line-height: 30px;
        }
    }
}


</style>