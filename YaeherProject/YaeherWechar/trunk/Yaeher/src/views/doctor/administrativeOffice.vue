<template>
    <div class="administrative-office padding-top">
        <mt-header fixed title="科室管理">
            <a @click="$router.push({path: '/doctor-user'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <router-link to="/alter-office" class="right-white">添加</router-link>
            </mt-button> 
        </mt-header>
        <div class="content">
            <ul class="office-list">
                <li  v-for="(item, index) in clinicList" :key="index" tag="li">
                    <router-link tag="div" :to="{path: '/alter-office', query: {id: 0, clinicID: item.clinicID, iIInessType: item.clinicName}}" v-if="item.checkResCode === ''">
                        <p>{{item.clinicName}} ({{item.clinicType | clinicSelect}})</p>
                        <i class="mint-cell-allow-right"></i>
                    </router-link> 
                    <div v-if="item.checkResCode === 'checking'" @click="hintClick('科室审核中，请耐心等候！')">
                        <p>{{item.clinicName}} ({{item.clinicType | clinicSelect}})</p>
                        <span class="clinicHint red-hint">{{item.checkRes}}</span>
                        <i class="mint-cell-allow-right"></i>
                    </div>
                    <div v-if="item.checkResCode === 'fail'" @click="hintClick('审核失败，请重新添加！')">
                        <p>{{item.clinicName}} ({{item.clinicType | clinicSelect}})</p>
                        <span class="clinicHint red-hint">{{item.checkRes}}</span>
                        <i class="mint-cell-allow-right"></i>
                    </div>                    
                    <router-link tag="div" :to="{path: '/alter-office', query: {id: item.id, clinicID: item.clinicID, iIInessType: item.clinicName}}" v-if="item.checkResCode === 'success'">
                        <p>{{item.clinicName}} ({{item.clinicType | clinicSelect}})</p>
                        <span class="clinicHint green-hint">{{item.checkRes}}</span>
                        <i class="mint-cell-allow-right"></i>
                    </router-link>  
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            clinicList: []
        }
    },
    filters: {
        clinicSelect (value) {
            const clinicType = ['无', '成人', '儿童']
            return clinicType[value]
        }
    },
    methods: {
        getUserInfo() { // 请求医生科室列表
            this.instance.doctorClinicApplyOutD({
            })
                .then((response) => {
                    const clinicList = response.data.result.item
                    if (clinicList) {
                        this.clinicList = clinicList
                    } else {
                        Toast('没有可以新增的科室了~')
                    }
                })
                .catch((error) => {
                }) 
        },
        hintClick (mes) {
            Toast(mes)
        }
    },
    mounted () {
        this.getUserInfo()
    },
    created () {
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.administrative-office {
    .office-list {margin: 10px;}
    .office-list li{font-size: $font-l; color: $color-afont;position: relative;}
    .clinicHint {position: absolute; right: 20px; top: 17px;}
    .mint-cell-allow-right::after {right: 10px;}
}

</style>