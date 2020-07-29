<template>
    <div class="add-subject padding-top">
        <mt-header fixed title="添加科室">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content add-box">
            <mt-radio
            class="openClose"
            title="科室属于"
            v-model="clinicType"
            :options="options3">
            </mt-radio>
            <div class="add-case">
                <p class="disease-title">科室</p>
                <mt-field label="科室名称" placeholder="请输入科室名称" v-model="clinicName"></mt-field>
                
            </div>
            <div v-if="id===0" class="okCase">
                <mt-button @click="addClinic" type="primary" class="mint-button--large">添加</mt-button>
            </div>
            <div v-if="id!=0" class="okCase">
                <mt-button @click="alterClinic" type="primary" class="mint-button--large">修改</mt-button>
            </div>
        </div>
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';
export default {
    data () {
        return {
            id: 0,
            orderSort: 0,
            disease: '',
            clinicName: '',
            options3: [
                {   
                    label: '儿童',
                    value: '2'
                },
                {
                    label: '成人',
                    value: '1'
                }
            ],
            clinicType: '2'
        }
    },
    methods: {
        alterClinic () { // 修改
            const id = this.id
            const clinicName = this.clinicName
            const orderSort = this.orderSort
            let clinicType = parseInt(this.clinicType)
            if (!clinicName) {
                Toast('科室名称不能为空')
                return
            }
            this.instance.updateClinicC({
                id,
                clinicName,
                clinicType,
                orderSort
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        Toast('修改成功')
                        this.$router.go(-1)
                    }
                })
                .catch((error) => {

                }) 
        },
        addClinic () { // 添加
            const clinicName = this.clinicName
            let clinicType = parseInt(this.clinicType)
            if (!clinicName) {
                Toast('科室名称不能为空')
                return
            }
            this.instance.createClinicC({
                clinicName,
                clinicType,
                orderSort: 0
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        Toast('添加成功')
                        this.$router.go(-1)
                    }
                })
                .catch((error) => {

                }) 
        },
        getClinicList () {
            const id = this.id
            this.instance.clinicByIdC({
                id
            })
                .then((response) => {
                    let clinicDetail = response.data.result.item
                    this.clinicName = clinicDetail.clinicName
                    this.clinicType = `${clinicDetail.clinicType}`
                })
                .catch((error) => {

                }) 
        },
        
    },
    mounted () {
        if (this.id != 0) this.getClinicList()
        
    },
    created () {
        let id = this.$route.query.id
        let orderSort = this.$route.query.orderSort
        if (id) this.id = parseInt(id)
        if (orderSort) this.orderSort = parseInt(orderSort)
        console.log(this.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.add-subject {
    .add-box {
        .mint-radiolist {
            padding: 10px;
        }
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
}


</style>