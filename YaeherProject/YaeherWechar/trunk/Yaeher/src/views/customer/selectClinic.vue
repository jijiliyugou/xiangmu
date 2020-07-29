<template>
    <div class="selectClinic padding-top">
        <mt-header fixed title="分配科室">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="arrange-case">

                <mt-checklist
                title="分配科室"
                v-model="value"
                :options="options2">
                </mt-checklist>
                
                <div class="set-ok">
                    <mt-button @click="selectClinic" type="primary" class="mint-button--large">保存</mt-button>
                </div>
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
            value: [],
            clicicArry: []
        }
    },
    methods: {
        getClinicList () {
            this.instance.clinicPageC({
            })
                .then((response) => {
                    let list = response.data.result.item
                    let clicicArry = new Array()
                    for (let i = 0; i< list.length; i++) {
                        const clinicType1 = ['无', '成人', '儿童']
                        list[i].clinicType = clinicType1[list[i].clinicType]
                        
                        let obj = {
                            label: `${list[i].clinicName}(${list[i].clinicType})`,
                            value: list[i].id
                        }
                        clicicArry.push(obj)
                    }
                    this.clicicArry = clicicArry
                })
                .catch((error) => {

                }) 
        },
        getDoctorClinic() { // 获取医生科室列表
            const doctorID = this.id
            this.instance.doctorClinicListByDoctorIDC({
                doctorID
            })
                .then((response) => {
                    let list = response.data.result.item
                    let clinicIDJSON = list.clinicIDJSON
                    let strArry = clinicIDJSON.split(',')
                    strArry.pop()
                    this.value = strArry

                })
                .catch((error) => {

                }) 
        },
        selectClinic () { // 提交分配科室信息
            let clinicIDJSON = this.value.join(',')
            const doctorID = this.id
            let _this = this
            this.instance.checkDoctorClinicC({
                doctorID,
                clinicIDJSON
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        if (response.data.result.msg === 'fail') {
                            MessageBox.confirm(response.data.result.item).then(action => {
                                this.submitClinic()
                            },function(){
                                console.log('取消了');
                            })
                        } else {
                            this.submitClinic()
                        }
                        
                        
                    }
                })
                .catch((error) => {

                }) 
            
        },
        submitClinic () { // 提交科室分配
            let clinicIDJSON = this.value.join(',')
            const doctorID = this.id
            let _this = this
            this.instance.createDoctorClinicC({
                doctorID,
                clinicIDJSON
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        Toast('分配成功')
                        this.$router.go(-1)
                    }
                })
                .catch((error) => {

                }) 
        }
    },
    computed: {
        options2() {
            let dataSlots = this.clicicArry
            return dataSlots
        },
    },    
    mounted () {
        this.getClinicList()
        this.getDoctorClinic()
    },
    created () {
        this.id = parseInt(this.$route.query.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.selectClinic {
    .set-ok {
        padding: 15px;
    }
}


</style>